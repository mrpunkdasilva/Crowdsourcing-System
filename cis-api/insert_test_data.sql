-- Insert data into 'users' table
INSERT INTO users (id, login, name, password) VALUES
('user1', 'alice', 'Alice Smith', 'password123'),
('user2', 'bob', 'Bob Johnson', 'securepass'),
('user3', 'charlie', 'Charlie Brown', 'mysecret');

-- Insert data into 'topics' table
INSERT INTO topics (id, title, description, created_at, created_by_id) VALUES
(1, 'Primeiro Tópico', 'Este é o primeiro tópico de discussão.', NOW(), 'user1'),
(2, 'Segundo Tópico', 'Um tópico sobre novas ideias.', NOW(), 'user2');

-- Insert data into 'ideas' table
INSERT INTO ideas (id, topic_id, title, description, created_at, created_by_id, vote_count) VALUES
(101, 1, 'Ideia 1.1', 'Uma ideia para o primeiro tópico.', NOW(), 'user1', 5),
(102, 1, 'Ideia 1.2', 'Outra ideia para o primeiro tópico.', NOW(), 'user2', 3),
(103, 2, 'Ideia 2.1', 'Ideia inicial para o segundo tópico.', NOW(), 'user3', 8);

-- Insert data into 'idea_votes' table
INSERT INTO idea_votes (idea_id, user_id, voted_at) VALUES
(101, 'user1', NOW()),
(101, 'user2', NOW()),
(101, 'user3', NOW()),
(102, 'user1', NOW()),
(102, 'user3', NOW()),
(103, 'user1', NOW()),
(103, 'user2', NOW()),
(103, 'user3', NOW());