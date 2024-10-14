 create table departments (
    id uuid primary key,
    name varchar(100) not null unique 
 );
 
 create table positions(
    id uuid primary key,
    title varchar(100) not null unique 
 );
 
create table employees
(
  id uuid primary key, 
  firstname varchar(50) not null,
  lastname varchar(50) not null,
  date_of_birth timestamp null,
  adress varchar(100) null,
  passport varchar(100) not null unique ,
  phone_number varchar(20) not null unique ,
  position_id varchar(30) not null,
  department_id varchar(50) not null ,
  salary numeric(10,2) not null,
  contract varchar(50) null ,
  age integer null
  FOREIGN KEY (position_id) REFERENCES positions(id) ON UPDATE CASCADE ON DELETE CASCADE,
  FOREIGN KEY (department_id) REFERENCES departments(id) ON UPDATE CASCADE ON DELETE CASCADE
);

create table clients
(
    id uuid primary key,
    firstname varchar(50) not null,
    lastname varchar(50) not null,
    date_of_birth timestamp null,
    adress varchar(100) null,
    passport varchar(100) not null unique,
    phone_number varchar(20) not null unique ,
    age int null
);

CREATE TABLE currency_codes (
    code CHAR(3) PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    symbol VARCHAR(5),
    country_region VARCHAR(100)
);

create table accounts
(
    account_number varchar(34) primary key,
    currency_code char(3) ,
    currency_name varchar(50) not null ,
    amount money not null,
    client_id uuid not null ,
    FOREIGN KEY(client_id) REFERENCES clients(id) ON DELETE CASCADE ON UPDATE CASCADE
    FOREIGN KEY(currency_code) REFERENCES currency_codes(code) ON DELETE CASCADE ON UPDATE CASCADE 
);

INSERT INTO departments
VALUES (
    uuid_generate_v4(), 'IT-отдел'
),
(
    uuid_generate_v4(), 'Отдел продаж'
),
(
    uuid_generate_v4(), 'Финансовый отдел',
),
(
    uuid_generate_v4(), 'HR'
),
(
    uuid_generate_v4(), 'Руководство'
);

INSERT INTO positions 
VALUES (
    uuid_generate_v4(), 'backend-dev'
),
(
    uuid_generate_v4(),'frontend-dev' 
),
(
    uuid_generate_v4(), 'тестировщик'
),
(
    uuid_generate_v4(), 'ux-дизайнер'
),
(
    uuid_generate_v4(), 'dev-ops'
),
(
    uuid_generate_v4(), 'бухгалтер'
);

INSERT INTO employees (id, firstname, lastname, date_of_birth, adress, passport, phone_number, position_id, department_id, salary, age)
VALUES
(
 uuid_generate_v4(), 'Сергей', 'Воробьев', '2001-01-03', 'г. Тирасполь', 'I-ПР234156', '77845635', '271586c3-e205-4b52-8168-fcc21b8de274', '3b5ad8f6-aa1c-4c0f-a0ed-fd3e2c14aeda', 8500.67, 23
),
(
 uuid_generate_v4(), 'Алексей', 'Скорцов', '1990-12-15', 'г. Бендеры', 'I-ПР294156', '77911233', 'cc2ff737-2cf7-423e-a1b6-c5f7e2838b60','3b5ad8f6-aa1c-4c0f-a0ed-fd3e2c14aeda', 6400.87, 34
),
(
 uuid_generate_v4(), 'Ольга', 'Пономарева', '2004-09-06', 'г. Тирасполь', 'I-ПР123456', '77799876', '674f7e15-a217-4fe1-926f-5716821382d7','3b5ad8f6-aa1c-4c0f-a0ed-fd3e2c14aeda', 5400.67, 20
),
(
 uuid_generate_v4(), 'Анастасия', 'Ковалева', '2005-03-12', 'г. Каменка', 'I-ПР234231', '77756432', '3a19a180-81e5-402e-b1c3-8d4c5dea41f0', '3b5ad8f6-aa1c-4c0f-a0ed-fd3e2c14aeda', 6000.23, 19
),
(
 uuid_generate_v4(), 'Александр', 'Трофимов', '1999-04-15', 'г. Тирасполь', 'I-ПР215678', '77734908', '271586c3-e205-4b52-8168-fcc21b8de274', '3b5ad8f6-aa1c-4c0f-a0ed-fd3e2c14aeda', 8500.67, 25
),
(
 uuid_generate_v4(), 'Олег', 'Обручков', '2000-01-29', 'г. Бендеры', 'I-ПР128903', '77845452', 'bf82b317-74a8-4958-bf30-6d798217a5ed','3b5ad8f6-aa1c-4c0f-a0ed-fd3e2c14aeda', 9000.67, 24
),
(
 uuid_generate_v4(), 'Алена', 'Гаврильченко', '1995-05-25', 'г. Тирасполь', 'I-ПР234763', '77788564', 'f1397181-a81d-4bd6-a7fc-2979f5af8b6f','f4e161d6-b062-4851-84d3-b52347f79269', 6000.50, 29
);

INSERT INTO clients (id, firstname, lastname, date_of_birth, adress, passport, phone_number, age)
VALUES
(
    uuid_generate_v4(), 'Иван', 'Петров', '1985-02-15', 'г. Москва, ул. Пушкина, д. 10', 'AB123456', '79991234567', 39
),
(
    uuid_generate_v4(), 'Екатерина', 'Смирнова', '1990-08-24', 'г. Санкт-Петербург, ул. Ленина, д. 3', 'CD234567', '79991112233',  34
),
(
    uuid_generate_v4(), 'Александр', 'Кузнецов', '1977-12-05', 'г. Казань, ул. Лермонтова, д. 5', 'EF345678', '79881112244', 46
),
(
    uuid_generate_v4(), 'Мария', 'Волкова', '2000-03-18', 'г. Новосибирск, ул. Чапаева, д. 8', 'GH456789', '79771112255', 24
),
(
    uuid_generate_v4(), 'Дмитрий', 'Медведев', '1995-11-30', 'г. Краснодар, ул. Кирова, д. 12', 'IJ567890', '79671112266', 28
),
(
    uuid_generate_v4(), 'Анна', 'Соколова', '1988-07-10', 'г. Екатеринбург, ул. Гагарина, д. 15', 'KL678901', '79561112277', 36
),
(
    uuid_generate_v4(), 'Николай', 'Иванов', '1999-09-25', 'г. Самара, ул. Победы, д. 9', 'MN789012', '79451112288', 25
);

INSERT INTO currency_codes (code, name, symbol, country_region) VALUES
('USD', 'Доллар США', '$', 'США'),
('EUR', 'Евро', '€', 'Еврозона'),
('RUB', 'Российский рубль', '₽', 'Россия'),
('GBP', 'Фунт стерлингов', '£', 'Великобритания'),
('JPY', 'Японская иена', '¥', 'Япония'),
('CHF', 'Швейцарский франк', '₣', 'Швейцария'),
('CNY', 'Китайский юань', '¥', 'Китай'),
('AUD', 'Австралийский доллар', '$', 'Австралия'),
('CAD', 'Канадский доллар', '$', 'Канада'),
('UAH', 'Гривна', '₴', 'Украина'),
('KZT', 'Казахстанский тенге', '₸', 'Казахстан'),
('BYN', 'Белорусский рубль', 'Br', 'Беларусь'),
('TRY', 'Турецкая лира', '₺', 'Турция'),
('INR', 'Индийская рупия', '₹', 'Индия'),
('BRL', 'Бразильский реал', 'R$', 'Бразилия'),
('ZAR', 'Южноафриканский рэнд', 'R', 'ЮАР'),
('MXN', 'Мексиканское песо', '$', 'Мексика'),
('KRW', 'Южнокорейская вона', '₩', 'Южная Корея'),
('AED', 'Дирхам ОАЭ', 'د.إ', 'ОАЭ'),
('RUP', 'Приднестровский рубль', 'р.', 'Приднестровье');

INSERT INTO accounts
VALUES
(
    'RU02 2200 1345 1234 5678 90','RUB', 'Российский рубль', 15000.00, '4bec0278-5ff9-4418-82db-22cdad8411e7'
),
(
    'RU45 3100 2345 6789 1234 56', 'USD', 'Доллар США', 500.00, '4bec0278-5ff9-4418-82db-22cdad8411e7'
),
(
    'MD78 4100 5432 9876 1234 89','EUR', 'Евро', 1200.00, '5b86c966-9587-4d70-a61e-b3bac2f7c159'
),
(
    'MD93 5200 6789 1234 5678 12','RUB', 'Российский рубль', 18000.00, '57df73f8-294a-4c6d-a9eb-ccb6c5a27d92'
),
(
    'UA12 6300 9876 5432 1234 45','USD', 'Доллар США', 350.50, '57df73f8-294a-4c6d-a9eb-ccb6c5a27d92'
),
(
    'RU57 7400 1234 5678 9876 78','RUP', 'Приднестровский рубль', 25000.75, '10302b3c-4bbe-4e7c-8b1a-75cb6cf06ec7'
),
(
    'RU88 8500 8765 4321 9876 23','EUR', 'Евро', 1500.00, '10302b3c-4bbe-4e7c-8b1a-75cb6cf06ec7'
),
(
    'MD31 9600 5432 1234 8765 56','UAH', 'Гривна', 1500.00, '4392166c-7152-43b2-9b72-0e1f1bc2b896'
);

SELECT clients.id, clients.firstname, clients.lastname, clients.date_of_birth, clients.phone_number, a.amount
FROM clients
INNER JOIN accounts a on clients.id = a.client_id
WHERE CAST(a.amount AS numeric) < 2000
ORDER BY CAST(a.amount AS numeric);

SELECT clients.id, clients.firstname, clients.lastname, clients.date_of_birth, clients.phone_number, a.amount
FROM clients
INNER JOIN accounts a on clients.id = a.client_id
WHERE a.amount = (SELECT MIN(amount) FROM accounts);

SELECT SUM(amount) AS clients_total_balance
FROM accounts;

SELECT clients.id, clients.firstname, clients.lastname, a.currency_name, a.currency_code, a.amount
FROM clients
INNER JOIN accounts a on clients.id = a.client_id;

SELECT * FROM clients ORDER BY(age) DESC;

SELECT clients.age, count(*) AS clients_with_same_age
FROM clients
GROUP BY age;

SELECT clients.age, COUNT(*) AS total_count
FROM clients
GROUP BY age
HAVING COUNT(*) > 1
ORDER BY age;

SELECT * FROM clients
LIMIT 5;



