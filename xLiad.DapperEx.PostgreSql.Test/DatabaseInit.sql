CREATE TABLE public."DictInfo"
(
    "DictID" serial NOT NULL,
    "DictName" character varying(100),
    "DictType" int,
	"Remark"  character varying(100),
	"CreateUserID" int,
	"CreateTime" timestamp,
	"OrderNum" int,
	"Status" int,
	"IsUserConfig" int,
	"Deleted" boolean,
    CONSTRAINT "DictInfo_pkey" PRIMARY KEY ("DictID")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."DictInfo"
    OWNER to postgres;



	CREATE TABLE public."News"
(
    "Id" serial NOT NULL,
    "Title" character varying(50) COLLATE pg_catalog."default",
    "Content" text COLLATE pg_catalog."default",
    "Author" jsonb,
    CONSTRAINT "News_pkey" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."News"
    OWNER to postgres;

	
	CREATE TABLE public."News2"
(
    "Id" serial NOT NULL,
    "Title" character varying(50) COLLATE pg_catalog."default",
    "Content" text COLLATE pg_catalog."default",
    "Author" jsonb,
    CONSTRAINT "News2_pkey" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."News2"
    OWNER to postgres;


INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( '这是名字', 100, '这是备注', 0, now(), 1, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( '创意长', 102, '修改个备注', 0, now(), 0, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( '技术副总裁', 104, '修改个备注', 0, now(), 1, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( '哈哈哈', 100, '嘿嘿嘿', 0, now(), 0, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( '哈哈哈', 100, '嘿嘿嘿', 0, now(), 1, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( '老总', 102, '老总，请加薪', 0, now(), 0, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( '老总秘书', 100, '老总秘书，请加薪', 0, now(), 1, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( 'PHP是', 104, '最好的语言', 0, now(), 0, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( 'Java是', 100, '最好的语言', 0, now(), 1, 0, null, false);
INSERT INTO public."DictInfo"(
	"DictName", "DictType", "Remark", "CreateUserID", "CreateTime", "OrderNum", "Status", "IsUserConfig", "Deleted")
	VALUES ( '康师傅', 100, null, 0, now(), 0, 0, null, false);

	
	INSERT INTO public."News"(
	"Title", "Content", "Author")
	VALUES ('今天天气不错', '今天天气真是不错', '{"Id":11,"Name":"金大侠","BirthDay":"2019-03-10T22:26:19.7686696+08:00"}'::jsonb);
	INSERT INTO public."News"(
	"Title", "Content", "Author")
	VALUES ('PHP是世界上最好的语言', 'PHP是世界上最好的语言，啦啦啦。', '{"Id":6,"Name":"帅哥王","BirthDay":"1993-03-10T22:26:19.7686696+08:00"}'::jsonb);
	INSERT INTO public."News"(
	"Title", "Content", "Author")
	VALUES ('Java是世界上最好的语言', 'Java是世界上最好的语言，啦啦啦。', '{"Id":7,"Name":"明浩大神","BirthDay":"1981-03-10T22:26:19.7686696+08:00"}'::jsonb);
	INSERT INTO public."News"(
	"Title", "Content", "Author")
	VALUES ('Python是世界上最好的语言', 'Python是世界上最好的语言，啦啦啦。', '{"Id":3,"Name":"张小牛","BirthDay":"1982-03-10T22:26:19.7686696+08:00"}'::jsonb);
	INSERT INTO public."News"(
	"Title", "Content", "Author")
	VALUES ('C++是世界上最好的语言', 'C++是世界上最好的语言，啦啦啦。', '{"Id":5,"Name":"晓斌校长","BirthDay":"1982-03-10T22:26:19.7686696+08:00"}'::jsonb);
	INSERT INTO public."News"(
	"Title", "Content", "Author")
	VALUES ('Js是世界上最好的语言', 'Js是世界上最好的语言，啦啦啦。', '{"Id":1,"Name":"建华大师","BirthDay":"1988-03-10T22:26:19.7686696+08:00"}'::jsonb);
	INSERT INTO public."News"(
	"Title", "Content", "Author")
	VALUES ('C是世界上最好的语言', 'C是世界上最好的语言，啦啦啦。', '{"Id":2,"Name":"延杰抵迪","BirthDay":"1990-03-10T22:26:19.7686696+08:00"}'::jsonb);
	INSERT INTO public."News"(
	"Title", "Content", "Author")
	VALUES ('Ruby是世界上最好的语言', 'Ruby是世界上最好的语言，啦啦啦。', '{"Id":8,"Name":"盼盼","BirthDay":"1991-03-10T22:26:19.7686696+08:00"}'::jsonb);




INSERT INTO public."News2"(
	"Title", "Content", "Author")
	VALUES ( '今天天气不错', '今天天气真是不错', '[{"Id":6,"Name":"帅哥王","BirthDay":"1993-03-06T21:53:49.8148044+08:00"},{"Id":7,"Name":"明浩大神","BirthDay":"1981-03-06T00:00:00+08:00"}]'::jsonb);
	INSERT INTO public."News2"(
	"Title", "Content", "Author")
	VALUES ( 'PHP是世界上最好的语言', 'PHP是世界上最好的语言，啦啦啦。', '[{"Id":3,"Name":"张小牛","BirthDay":"1982-03-06T21:52:50.8898044+08:00"},{"Id":5,"Name":"晓斌校长","BirthDay":"1982-03-06T00:00:00+08:00"}]'::jsonb);
	INSERT INTO public."News2"(
	"Title", "Content", "Author")
	VALUES ( 'Java是世界上最好的语言', 'Java是世界上最好的语言，啦啦啦。', '[{"Id":1,"Name":"建华大师","BirthDay":"1988-03-06T21:52:50.8898044+08:00"},{"Id":2,"Name":"延杰抵迪","BirthDay":"1990-03-06T00:00:00+08:00"}]'::jsonb);
	INSERT INTO public."News2"(
	"Title", "Content", "Author")
	VALUES ( 'Python是世界上最好的语言', 'Python是世界上最好的语言，啦啦啦。', '[{"Id":8,"Name":"盼盼","BirthDay":"1991-03-06T21:52:50.8898044+08:00"},{"Id":9,"Name":"龙龙","BirthDay":"1992-03-06T00:00:00+08:00"}]'::jsonb);
	INSERT INTO public."News2"(
	"Title", "Content", "Author")
	VALUES ( 'C++是世界上最好的语言', 'C++是世界上最好的语言，啦啦啦。', '[{"Id":6,"Name":"帅哥王","BirthDay":"1993-03-06T21:52:50.8898044+08:00"},{"Id":7,"Name":"明浩大神","BirthDay":"1981-03-06T00:00:00+08:00"}]'::jsonb);
	INSERT INTO public."News2"(
	"Title", "Content", "Author")
	VALUES ( 'Js是世界上最好的语言', 'Js是世界上最好的语言，啦啦啦。', '[{"Id":3,"Name":"张小牛","BirthDay":"1982-03-06T21:52:50.8898044+08:00"},{"Id":5,"Name":"晓斌校长","BirthDay":"1982-03-06T00:00:00+08:00"}]'::jsonb);
	INSERT INTO public."News2"(
	"Title", "Content", "Author")
	VALUES ( 'C是世界上最好的语言', 'C是世界上最好的语言，啦啦啦。', '[{"Id":1,"Name":"建华大师","BirthDay":"1988-03-06T21:52:50.8898044+08:00"},{"Id":2,"Name":"延杰抵迪","BirthDay":"1990-03-06T00:00:00+08:00"}]'::jsonb);
	INSERT INTO public."News2"(
	"Title", "Content", "Author")
	VALUES ( 'Ruby是世界上最好的语言', 'Ruby是世界上最好的语言，啦啦啦。', '[{"Id":8,"Name":"盼盼","BirthDay":"1991-03-06T21:52:50.8898044+08:00"},{"Id":9,"Name":"龙龙","BirthDay":"1992-03-06T00:00:00+08:00"}]'::jsonb);
