package jalau.cis.services;

import jalau.cis.services.mybatis.MySqlDBService;
import java.io.FileInputStream;

public class ServicesFacade {
    private static ServicesFacade instance;


    private boolean configured = false;
    private UsersService dbService;

    public static synchronized ServicesFacade getInstance () {
        if (instance == null) {
            instance = new ServicesFacade();
        }
        return instance;
    }
    private ServicesFacade() {
    }

    public UsersService getUsersService() throws Exception{
        checkInit();
        return dbService;
    }

    private void checkInit() throws Exception{
        if (!configured) throw new Exception("Facade is not configured....");
    }

    public synchronized void init(String configFilePath) throws Exception {
        if (configured) {
            return;
        }
        try {
            dbService = new MySqlDBService(configFilePath);
            configured = true;
        }
        catch (Exception ex) {
            System.out.printf("Cannot connect to MySQL [%s]\n", ex.getMessage());
            configured = false;
            throw ex;
        }
    }

}
