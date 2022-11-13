﻿create database CARSALESSYSTEM
use CARSALESSYSTEM
go

set dateformat dmy
--NGUOIDUNG
CREATE TABLE ACCOUNT(
		USERNAME VARCHAR(100) NOT NULL,
		PASS VARCHAR(100) NOT NULL,
		TYPE_USER INT NOT NULL,
		CONSTRAINT PK_ND PRIMARY KEY(USERNAME)
)

--NHANVIEN
CREATE TABLE EMPLOYEE
(
		EMP_ID VARCHAR(4) NOT NULL,
		EMP_NAME NVARCHAR(100),
		EMP_ACCOUNT VARCHAR(100) NOT NULL,
		EMP_DATE_OF_BIRTH DATE,
		GENDER NVARCHAR(10),
		PHONE VARCHAR(20),
		EMP_ADDRESS NVARCHAR(50),
		DATE_OF_WORD DATE,
		SALARY MONEY,
		IMG VARBINARY(MAX),
		CONSTRAINT PK_NV PRIMARY KEY(EMP_ID)
)
--KHACHHANG
CREATE SEQUENCE IDKH
AS INT
START WITH 01
INCREMENT BY 1;

CREATE TABLE CUSTOMER(
		CUS_ID VARCHAR(4) NOT NULL
			CONSTRAINT DF_KHACHHANG
			DEFAULT('KH' +RIGHT('0' + CAST(NEXT VALUE FOR IDKH AS VARCHAR(10)), 2)), -- NO 
		CUS_NAME NVARCHAR(100) NOT NULL,
		CUS_ACCOUNT VARCHAR(100) NOT NULL,
		CUS_DATE_OF_BIRTH DATE,
		GENDER NVARCHAR(10),
		PHONE VARCHAR(20) NOT NULL,
		CUS_ADDRESS NVARCHAR(50) NOT NULL,
		REGIST_DATE DATE,
		REVENUE MONEY,
		PRODUCT_NUMBER INT,
		IMG VARBINARY(MAX),
		CONSTRAINT PK_KH PRIMARY KEY(CUS_ID)
)

--SANPHAM
CREATE TABLE PRODUCT(
		PRO_ID VARCHAR(4) NOT NULL,
		PRO_NAME NVARCHAR(100) NOT NULL,
		STORAGE_NUMBER INT,
		SELL_NUMBER INT,
		HISTORICAL_COST MONEY,
		PRICE MONEY NOT NULL,
		DESCRIPTIONS NVARCHAR(400),
		TYPEPRO_ID VARCHAR(4),
		PRODUCER_ID VARCHAR(4),
		IMG VARBINARY(MAX) NOT NULL,
		CONSTRAINT PK_SP PRIMARY KEY(PRO_ID)
)

--NHASANXUAT	
CREATE TABLE PRODUCER(
		PRODUCER_ID VARCHAR(4) NOT NULL,
		PRODUCER_NAME VARCHAR(40),
		CONSTRAINT PK_NSX PRIMARY KEY(PRODUCER_ID)
)

--LOAISANPHAM
CREATE TABLE TYPEPRODUCT(
		TYPEPRODUCT_ID VARCHAR(4) NOT NULL,
		TYPEPRODUCT_NAME NVARCHAR(100),
		PRODUCER_ID VARCHAR(4),
		CONSTRAINT PK_LOAISP PRIMARY KEY(TYPEPRODUCT_ID)
)



--HOADONMUAHANG
CREATE TABLE SELLBILL(
		SB_ID VARCHAR(4) NOT NULL,
		SB_DATEB DATETIME,
		CUSTOMER_ID VARCHAR(4) NOT NULL,
		PRO_ID VARCHAR(4),
		EMPLOYEE_ID VARCHAR(4),
		TOTAL_PRICE MONEY,
		CONSTRAINT PK_HOADON PRIMARY KEY(SB_ID)
)

--HOADONBAOTRI
CREATE TABLE MAINTENANCEBILL(
		MB_ID VARCHAR(4) NOT NULL,
		MB_DATE DATETIME,
		CUSTOMER_ID VARCHAR(4) NOT NULL,
		PRO_ID VARCHAR(4) NOT NULL,
		EMPLOYEE_ID VARCHAR(4),
		TOTALFEE MONEY,
		CONSTRAINT PK_HOADONBT PRIMARY KEY(MB_ID)
)

--KHOA NGOAI KHACHHANG & NHANVIEN
ALTER TABLE EMPLOYEE ADD CONSTRAINT FK_TAIKHOANNV_TAIKHOAN FOREIGN KEY(EMP_ACCOUNT) REFERENCES ACCOUNT(USERNAME);
ALTER TABLE CUSTOMER ADD CONSTRAINT FK_TAIKHOANKH_TAIKHOAN FOREIGN KEY(CUS_ACCOUNT) REFERENCES ACCOUNT(USERNAME);

--KHOA NGOAI SANPHAM
ALTER TABLE PRODUCT ADD CONSTRAINT FK_MALOAISP_MALOAI FOREIGN KEY(TYPEPRO_ID) REFERENCES TYPEPRODUCT(TYPEPRODUCT_ID);
ALTER TABLE PRODUCT ADD CONSTRAINT FK_MANSXSP_MANSX FOREIGN KEY(PRODUCER_ID) REFERENCES PRODUCER(PRODUCER_ID);

--KHOA NGOAI LOAISANPHAM
ALTER TABLE TYPEPRODUCT ADD CONSTRAINT FK_MANSX FOREIGN KEY(PRODUCER_ID) REFERENCES PRODUCER(PRODUCER_ID);


--KHOA NGOAI HOADONMUAHANG
ALTER TABLE SELLBILL ADD CONSTRAINT FK_MAKHMH FOREIGN KEY(CUSTOMER_ID) REFERENCES CUSTOMER(CUS_ID);
ALTER TABLE SELLBILL ADD CONSTRAINT FK_MANVMH FOREIGN KEY(EMPLOYEE_ID) REFERENCES EMPLOYEE(EMP_ID);
ALTER TABLE SELLBILL ADD CONSTRAINT FK_MASPMH FOREIGN KEY(PRO_ID) REFERENCES PRODUCT(PRO_ID);

--KHOA NGOAI HOADONBAOTRI
ALTER TABLE MAINTENANCEBILL ADD CONSTRAINT FK_MAKHBT FOREIGN KEY(CUSTOMER_ID) REFERENCES CUSTOMER(CUS_ID);
ALTER TABLE MAINTENANCEBILL ADD CONSTRAINT FK_MANVBT FOREIGN KEY(EMPLOYEE_ID) REFERENCES EMPLOYEE(EMP_ID);
ALTER TABLE MAINTENANCEBILL ADD CONSTRAINT FK_MASPBT FOREIGN KEY(PRO_ID) REFERENCES PRODUCT(PRO_ID);
