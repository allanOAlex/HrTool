-- Database: internalhrtool

-- DROP DATABASE IF EXISTS internalhrtool;

CREATE DATABASE internalhrtool
    WITH
    OWNER = allan
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;

GRANT ALL ON DATABASE internalhrtool TO allan;

GRANT TEMPORARY, CONNECT ON DATABASE internalhrtool TO PUBLIC;