# Tarefa 7


## 1. Modelo de Dados Atual (MySQL)

Os dados existentes estão distribuídos entre dois contextos de aplicação principais, `users-api` e `cis-api`, mas residem em um único banco de dados MySQL chamado `sd3`.

### Dados de Usuários (`users-api` e `cis` CLI)

O `users-api`, uma aplicação Java Spring Boot, e o `cis` CLI, uma aplicação Java de linha de comando, gerenciam a tabela `users`, que contém informações de autenticação e perfil dos usuários.

-   **Tabela `users`:**
    -   `id` (VARCHAR(36), Chave Primária)
    -   `name` (VARCHAR(200))
    -   `login` (VARCHAR(20), Único)
    -   `password` (VARCHAR(100))

### Dados CIS (`cis-api`)

O `cis-api`, uma aplicação C# ASP.NET Core, gerencia tópicos, ideias e votos.

-   **Tabela `topics`:**
    -   `id` (INT, Chave Primária)
    -   `title` (TEXT)
    -   `description` (TEXT)
    -   `created_at` (DATETIME)
    -   `created_by_id` (VARCHAR(36), Chave Estrangeira para `users.id`)

-   **Tabela `ideas`:**
    -   `id` (INT, Chave Primária)
    -   `topic_id` (INT, Chave Estrangeira para `topics.id`)
    -   `title` (TEXT)
    -   `description` (TEXT)
    -   `created_at` (DATETIME)
    -   `created_by_id` (VARCHAR(36), Chave Estrangeira para `users.id`)
    -   `vote_count` (BIGINT)

-   **Tabela `idea_votes`:** (Tabela de junção para relacionamento muitos-para-muitos)
    -   `idea_id` (INT, Chave Primária, Chave Estrangeira para `ideas.id`)
    -   `user_id` (VARCHAR(36), Chave Primária, Chave Estrangeira para `users.id`)
    -   `voted_at` (DATETIME)

## 2. Modelo de Dados Proposto (MongoDB)

A estrutura baseada em documentos do MongoDB nos permite criar um modelo mais intuitivo e com melhor desempenho, incorporando dados relacionados diretamente nos documentos. Essa abordagem reduz a necessidade de *joins* complexos, que são comuns em bancos de dados relacionais.

### Coleção `users`

A coleção `users` terá um mapeamento direto 1:1 da tabela `users` do MySQL.

```json
{
  "_id": "4e542f0d-f9ff-47c3-88e2-90f6e82b0ac9",
  "name": "Javier Roca",
  "login": "jroca",
  "password": "hashed_password_here"
}
```

### Coleção `topics`

Esta coleção será o ponto central do novo modelo. Cada documento `topic` incorporará suas `ideas` associadas, e cada `idea` incorporará informações sobre seu criador e os usuários que votaram nela. Essa desnormalização é altamente eficaz porque as ideias são quase sempre consultadas no contexto de um tópico.

```json
{
  "_id": ObjectId("67f..."),
  "title": "Improving Remote Work",
  "description": "A collection of ideas to enhance the remote work experience.",
  "createdAt": ISODate("2025-10-14T10:00:00Z"),
  "createdBy": {
    "userId": "4e542f0d-f9ff-47c3-88e2-90f6e82b0ac9",
    "login": "jroca"
  },
  "ideas": [
    {
      "ideaId": 1,
      "title": "Virtual Coffee Breaks",
      "description": "Organize daily 15-minute optional video calls for informal chats.",
      "createdAt": ISODate("2025-10-14T11:30:00Z"),
      "createdBy": {
        "userId": "ebe2ce10-9c30-4b55-8609-445802b61f34",
        "login": "mlopez"
      },
      "voteCount": 2,
      "votedBy": [
        {
          "userId": "4e542f0d-f9ff-47c3-88e2-90f6e82b0ac9",
          "login": "jroca",
          "votedAt": ISODate("2025-10-14T12:00:00Z")
        },
        {
          "userId": "8f170a57-f2d9-43d8-b0b1-4b9eac9ae77a",
          "login": "another_user",
          "votedAt": ISODate("2025-10-14T12:05:00Z")
        }
      ]
    }
  ]
}
```

**Justificativa do Design:**
-   **Incorporação (Embedding):** As `ideas` são incorporadas dentro dos `topics` para garantir que uma única consulta possa recuperar um tópico e todo o seu conteúdo relacionado.
-   **Desnormalização Parcial:** Em vez de incorporar o documento completo do usuário, incorporamos apenas os campos necessários (`userId`, `login`) para manter os documentos leves e evitar *lookups* adicionais para cenários de exibição comuns.

## 3. Estratégia de Migração

Dada a natureza interconectada dos dados e o tamanho deste projeto pessoal, uma migração do tipo **Big Bang** é a abordagem mais direta e recomendada. Esta estratégia envolve uma janela de manutenção programada durante a qual todos os dados são migrados de uma só vez.

-   **Vantagens:** Mais simples de implementar, menor risco de inconsistência de dados e não requer lógica de sincronização complexa.
-   **Desvantagens:** Requer uma janela de manutenção (tempo de inatividade) para a migração.

Uma migração faseada seria excessivamente complexa para este caso de uso, pois exigiria que ambas as APIs lidassem com duas fontes de dados simultaneamente ou tivessem um mecanismo de sincronização de dados em vigor.

## 4. Plano de Migração Detalhado

Este plano detalha os passos para executar a migração.

### Fase 1: Preparação

1.  **Configurar o Ambiente MongoDB:**
    Inicie uma instância do MongoDB usando Docker Compose. Crie um arquivo `docker-compose.yml` (se ainda não tiver um para o MongoDB):
    ```yaml
    version: '3.8'
    services:
      mongodb:
        image: mongo:latest
        container_name: mongodb_cis
        ports:
          - "27017:27017"
        volumes:
          - mongo_data:/data/db
        environment:
          MONGO_INITDB_ROOT_USERNAME: root
          MONGO_INITDB_ROOT_PASSWORD: mongopass

    volumes:
      mongo_data:
    ```
    Execute `docker-compose up -d` para iniciar o contêiner.

2.  **Escolher Ferramentas de Migração:**
    Um script personalizado é a melhor escolha para esta migração, pois requer a transformação de dados de uma estrutura relacional para uma estrutura de documento aninhada. **Python** com as bibliotecas `mysql-connector-python` e `pymongo` é uma excelente opção.

3.  **Fazer Backup do Banco de Dados MySQL:**
    Antes de iniciar, crie um backup completo do banco de dados `sd3`.
    ```bash
    mysqldump -u root -p sd3 > sd3_backup.sql
    ```

### Fase 2: Execução (Scripts de Migração)

A migração será realizada por um script Python.

#### **Passo 1: Instalar Bibliotecas Python**
```bash
pip install mysql-connector-python pymongo
```

#### **Passo 2: Script de Migração**
O script Python a seguir descreve a lógica para conectar-se a ambos os bancos de dados, extrair os dados, transformá-los e carregá-los no MongoDB.

```python
import mysql.connector
from pymongo import MongoClient
from datetime import datetime

# --- Database Configurations ---
MYSQL_CONFIG = {
    'user': 'root',
    'password': 'sd5',
    'host': '127.0.0.1',
    'port': '3307',
    'database': 'sd3'
}
MONGO_CONFIG = {
    'host': 'localhost',
    'port': 27017,
    'username': 'root',
    'password': 'mongopass',
    'db_name': 'cis_db'
}

def migrate_data():
    """
    Connects to MySQL, transforms data, and inserts it into MongoDB.
    """
    try:
        # Connect to MySQL
        mysql_conn = mysql.connector.connect(**MYSQL_CONFIG)
        mysql_cursor = mysql_conn.cursor(dictionary=True)
        print("Successfully connected to MySQL.")

        # Connect to MongoDB
        mongo_client = MongoClient(
            host=MONGO_CONFIG['host'],
            port=MONGO_CONFIG['port'],
            username=MONGO_CONFIG['username'],
            password=MONGO_CONFIG['password']
        )
        mongo_db = mongo_client[MONGO_CONFIG['db_name']]
        print("Successfully connected to MongoDB.")

        # Clean up previous migration data
        mongo_db.users.delete_many({})
        mongo_db.topics.delete_many({})
        print("Cleared existing collections in MongoDB.")

        # 1. Migrate Users
        mysql_cursor.execute("SELECT id, name, login, password FROM users")
        users = mysql_cursor.fetchall()
        if users:
            # In MongoDB, the '_id' field is the primary key.
            # We will use the existing user ID for this.
            user_documents = [{'_id': u['id'], **u} for u in users]
            mongo_db.users.insert_many(user_documents)
            print(f"Migrated {len(users)} users.")

        # 2. Migrate Topics with Embedded Ideas
        mysql_cursor.execute("SELECT * FROM topics")
        topics = mysql_cursor.fetchall()
        
        for topic in topics:
            # Fetch createdBy user for the topic
            mysql_cursor.execute("SELECT id, login FROM users WHERE id = %s", (topic['created_by_id'],))
            topic_creator = mysql_cursor.fetchone()

            # Fetch ideas for the current topic
            mysql_cursor.execute("SELECT * FROM ideas WHERE topic_id = %s", (topic['id'],))
            ideas = mysql_cursor.fetchall()
            
            idea_documents = []
            for idea in ideas:
                # Fetch createdBy user for the idea
                mysql_cursor.execute("SELECT id, login FROM users WHERE id = %s", (idea['created_by_id'],))
                idea_creator = mysql_cursor.fetchone()

                # Fetch votes for the current idea
                mysql_cursor.execute('''
                    SELECT v.user_id, v.voted_at, u.login 
                    FROM idea_votes v 
                    JOIN users u ON v.user_id = u.id 
                    WHERE v.idea_id = %s
                ''', (idea['id'],))
                votes = mysql_cursor.fetchall()

                idea_doc = {
                    "ideaId": idea['id'],
                    "title": idea['title'],
                    "description": idea['description'],
                    "createdAt": idea['created_at'],
                    "createdBy": {
                        "userId": idea_creator['id'],
                        "login": idea_creator['login']
                    } if idea_creator else None,
                    "voteCount": idea['vote_count'],
                    "votedBy": [
                        {
                            "userId": v['user_id'],
                            "login": v['login'],
                            "votedAt": v['voted_at']
                        } for v in votes
                    ]
                }
                idea_documents.append(idea_doc)

            topic_document = {
                "title": topic['title'],
                "description": topic['description'],
                "createdAt": topic['created_at'],
                "createdBy": {
                    "userId": topic_creator['id'],
                    "login": topic_creator['login']
                } if topic_creator else None,
                "ideas": idea_documents
            }
            mongo_db.topics.insert_one(topic_document)

        print(f"Migrated {len(topics)} topics with their embedded ideas and votes.")

    except mysql.connector.Error as err:
        print(f"MySQL Error: {err}")
    except Exception as e:
        print(f"An error occurred: {e}")
    finally:
        if 'mysql_conn' in locals() and mysql_conn.is_connected():
            mysql_cursor.close()
            mysql_conn.close()
            print("MySQL connection closed.")
        if 'mongo_client' in locals():
            mongo_client.close()
            print("MongoDB connection closed.")

if __name__ == "__main__":
    migrate_data()
```

### Fase 3: Pós-Migração

1.  **Validação dos Dados:**
    -   Execute consultas em ambos os bancos de dados para comparar as contagens de usuários, tópicos e ideias.
    -   Inspecione manualmente alguns documentos complexos no MongoDB para garantir que a estrutura esteja correta e todos os dados estejam presentes.

2.  **Alterações no Código das Aplicações:**
    As aplicações `users-api` (Java) e `cis-api` (C#) precisarão ser atualizadas para interagir com o MongoDB.

    -   **`users-api` (Java/Spring Boot):**
        -   **Remover:** A dependência `spring-boot-starter-data-jpa` e o conector MySQL (`mysql-connector-j`).
        -   **Adicionar:** A dependência `spring-boot-starter-data-mongodb` no `pom.xml`.
        -   **Atualizar:** A classe `UserRepository` para usar `MongoRepository` e a entidade `DbUser` com anotações `@Document` do Spring Data MongoDB.
        -   **Configurar:** As propriedades de conexão do MongoDB no `application.properties`.

    -   **`cis-api` (C#/.NET):**
        -   **Remover:** A dependência `Pomelo.EntityFrameworkCore.MySql` e as referências ao Entity Framework Core para MySQL.
        -   **Adicionar:** O pacote NuGet oficial do MongoDB .NET Driver (`MongoDB.Driver`).
        -   **Reescrever:** A lógica de acesso a dados nos repositórios (`TopicRepository`, etc.) para usar a API do MongoDB .NET Driver para manipulação de documentos. Isso incluirá a criação de classes de modelo para representar os documentos do MongoDB.
        -   **Configurar:** A string de conexão do MongoDB no `appsettings.json`.

3.  **Cutover da Aplicação:**
    -   Implante as versões atualizadas do `users-api` e `cis-api` com as novas configurações de banco de dados.
    -   Teste exaustivamente todas as funcionalidades da aplicação para garantir que funcionem conforme o esperado com o MongoDB.

4.  **Desativação do MySQL:**
    -   Após um período de operação estável, o contêiner do banco de dados MySQL pode ser desligado com segurança e o volume de dados removido.

## 5. Considerações Adicionais

-   **Indexação:** Para garantir um bom desempenho de consulta no MongoDB, crie índices nos campos frequentemente consultados. Para este modelo, considere índices em:
    -   `users.login` (índice único)
    -   `topics.createdBy.userId`
    -   `topics.ideas.ideaId`
-   **Transações:** Embora este script de migração execute operações sequencialmente, um cenário de produção pode exigir que a migração de um único tópico e seus subdocumentos seja envolvida em uma transação para garantir atomicidade. O MongoDB suporta transações multi-documento.
-   **Tratamento de Erros e Rollback:** O script fornecido inclui tratamento básico de erros. Um script de nível de produção deve ter um registro mais robusto e um plano claro de *rollback* (por exemplo, restaurar o backup do MySQL e limpar as coleções do MongoDB).