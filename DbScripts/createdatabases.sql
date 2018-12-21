CREATE USER lab_user WITH ENCRYPTED PASSWORD 'lab_pass';

-- payments
CREATE DATABASE lab_netmicro_payments;
GRANT ALL PRIVILEGES ON DATABASE lab_netmicro_payments TO lab_user;

--hangfire
CREATE DATABASE lab_netmicro_jobs;
GRANT ALL PRIVILEGES ON DATABASE lab_netmicro_jobs TO lab_user;

--policy
CREATE DATABASE lab_netmicro_policy;
GRANT ALL PRIVILEGES ON DATABASE lab_netmicro_policy TO lab_user;

--pricing
CREATE DATABASE lab_netmicro_pricing;
GRANT ALL PRIVILEGES ON DATABASE lab_netmicro_pricing TO lab_user;


