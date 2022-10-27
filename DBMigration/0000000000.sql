-- Employee table
CREATE TABLE "Employee"(
	"employeeId" serial PRIMARY KEY NOT NULL,
	"firstName" varchar(50) NOT NULL,
	"lastName" varchar(50) NOT NULL,
	"email" varchar(200) NOT NULL,
	"phoneNumber" varchar(10) NOT NULL,
	"pictureUrl" varchar(255) NOT NULL,
	"address" varchar(50) NOT NULL,
	"address2" varchar(50) NOT NULL,
	"city" varchar(50) NOT NULL,
	"zip" varchar(50) NOT NULL,
	"employeeState" varchar(2) NOT NULL,
	"created" date NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar(50) NOT NULL,
	"rowVer" UUID NOT NULL
);
-- create projects table
CREATE TABLE "Project"(
	"projectId" SERIAL PRIMARY KEY NOT NULL,
	"projectName" varchar(50) NOT NULL UNIQUE,
	"startDate" timestamp NOT NULL,
	"endDate" timestamp,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar(50) NOT NULL,
	"rowVer" UUID NOT NULL
);
 -- Employee Project Table
CREATE TABLE "EmployeeProject"(
	"employeeProjectId" serial PRIMARY KEY NOT NULL,
	"employeeId" int NOT NULL,
	"projectId" int NOT NULL,
	"startDate" timestamp NOT NULL,
	"endDate" timestamp,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar(50) NOT NULL,
	"RowVer" timestamp NOT NULL, 
	CONSTRAINT "FK_employeeId" FOREIGN KEY ("employeeId")
	REFERENCES "Employee"("employeeId"),
	CONSTRAINT "FK_projectId" FOREIGN KEY ("projectId")
	REFERENCES "Project"("projectId")
);
--Create certificates table
CREATE TABLE "Certificate"(
	"certificateId" serial PRIMARY KEY,
	"certificateName" varchar(50) NOT NULL UNIQUE,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar(50) NOT NULL,
	"rowVer" UUID NOT NULL
);
--Creat EmployeeCertificate table
CREATE TABLE "EmployeeCertificate"(
	"employeeCertificateId" SERIAL PRIMARY KEY,
	"certificateId" int NOT NULL,
	"employeeId" int NOT NULL,
	"awardedDate" timestamp NOT NULL,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar(50) NOT NULL,
	"rowVer" UUID NOT NULL,
	CONSTRAINT "FK_employeeId" FOREIGN KEY ("employeeId")
	REFERENCES "Employee"("employeeId"),
	CONSTRAINT "FK_certificateId" FOREIGN KEY ("certificateId")
	REFERENCES "Certificate" ("certificateId")
);
--Create a role table
CREATE TABLE "Role"(
	"roleId" SERIAL PRIMARY KEY,
	"roleName" varchar (50) NOT NULL UNIQUE,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar (50) NOT NULL,
	"rowVer" UUID NOT NULL
);
--create employer table
CREATE TABLE "Employer"(
	"employerId" SERIAL PRIMARY KEY,
	"employerName" varchar (50) NOT NULL UNIQUE,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar (50) NOT NULL,
	"rowVer" UUID NOT NULL
);
--create employees roles table
CREATE TABLE "EmployeeRole"(
	"employeeRoleId" SERIAL PRIMARY KEY,
	"employeeId" int NOT NULL,
	"roleId" int NOT NULL,
	"startDate" timestamp NOT NULL,
	"endDate" timestamp,
	"employerId" int,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar (50) NOT NULL,
	"rowVer" UUID NOT NULL,
	CONSTRAINT "FK_EmployeeId" FOREIGN KEY ("employeeId")
	REFERENCES "Employee" ("employeeId"),
	CONSTRAINT "FK_RoleId" FOREIGN KEY ("roleId")
	REFERENCES "Role" ("roleId"),
	CONSTRAINT "FK_EmployerId" FOREIGN KEY ("employerId")
	REFERENCES "Employer" ("employerId")
);	
--creates skills table
CREATE TABLE "Skill"(
	"skillId" SERIAL PRIMARY KEY,
	"skillName" varchar (50) NOT NULL UNIQUE,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar (50) NOT NULL,
	"rowVer" UUID NOT NULL
);
--creates employees skill table
CREATE TABLE "EmployeeSkill"(
	"employeeSkillId" SERIAL PRIMARY KEY,
	"employeeId" int NOT NULL,
	"skillId" int NOT NULL,
	"created" timestamp NOT NULL,
	"updated" timestamp NOT NULL,
	"createdBy" varchar (50) NOT NULL,
	"rowVer" UUID NOT NULL,
	CONSTRAINT "FK_employeeId" FOREIGN KEY ("employeeId")
	REFERENCES "Employee"("employeeId"),
	CONSTRAINT "FK_skillId" FOREIGN KEY ("skillId")
	REFERENCES "Skill" ("skillId")
);