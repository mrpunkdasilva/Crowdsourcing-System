package jalau.cis.services.mybatis;

import com.mysql.cj.Session;
import jalau.cis.models.User;
import jalau.cis.services.UsersService;
import org.apache.ibatis.session.SqlSession;
import org.apache.ibatis.session.SqlSessionFactory;
import org.apache.ibatis.session.SqlSessionFactoryBuilder;

import org.apache.ibatis.session.SqlSessionFactoryBuilder;
import org.apache.ibatis.io.Resources;

import java.io.InputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class MySqlDBService implements UsersService {

    private SqlSessionFactory factory;

    private interface SessionOperation<T> {
        public T execute(SqlSession session, UserMapper mapper) throws Exception;
    }

    public MySqlDBService(String configFilePath) {
        try {
            InputStream inputStream = Resources.getResourceAsStream(configFilePath);
            factory = new SqlSessionFactoryBuilder().build(inputStream);
            factory.getConfiguration().addMapper(UserMapper.class);
        } catch (IOException e) {
            throw new RuntimeException("Error building SqlSessionFactory from " + configFilePath, e);
        }
    }

    private <T> T execute(SessionOperation<T> operation) throws Exception {
        SqlSession session = factory.openSession();
        UserMapper userMapper = session.getMapper(UserMapper.class);
        System.out.println("Opening Session");
        try {
            return operation.execute(session, userMapper);
        }
        catch (Exception ex)
        {
            System.out.printf("Error on SQL operation [%s]\n", ex.getMessage());
            throw  ex;
        }
        finally {
            System.out.println("Closing Session");
            session.close();
        }
    }

    @Override
    public int deleteUser(String id) throws Exception{
        return execute((session, userMapper) -> {
            try {
                int count = userMapper.delete(id);
                session.commit();
                return count;
            }
            catch (Exception ex) {
                session.rollback();
                throw ex;
            }
        });
    }

    @Override
    public List<User> getUsers() throws Exception {
        return execute( (session, userMapper) -> {
             var data  = userMapper.getAllUsers();
             if (data != null) {
                return new ArrayList<>(data.values());
              }
                else
                {
                    return new ArrayList<>();
                }
            });
    }

    @Override
    public void createUser(User user) throws Exception{
        execute((session, userMapper) -> {
            try {
                userMapper.createUser(user);
                session.commit();
                return 0;
            }
            catch (Exception ex) {
                session.rollback();
                throw  ex;
            }
        });

    }

    @Override
    public void updateUser(User user) throws Exception{
        execute((session, userMapper) -> {
            try {
                var userMap = userMapper.getUserById(user.getId());
                if (userMap.containsKey(user.getId())) {
                    User userToUpdate = user.cloneFrom(userMap.get(user.getId()));
                    userMapper.updateUser(userToUpdate);
                    session.commit();
                    return 0;
                }
                else {
                    throw new Exception("User does not exist");
                }
            }
            catch (Exception ex) {
                session.rollback();
                throw ex;
            }
        });
    }
}
