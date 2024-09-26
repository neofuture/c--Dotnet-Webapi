create table webapi.users
(
    id       bigint unsigned auto_increment
        primary key,
    name     varchar(255) not null,
    email    varchar(255) not null,
    password varchar(255) not null,
    constraint id
        unique (id)
);

