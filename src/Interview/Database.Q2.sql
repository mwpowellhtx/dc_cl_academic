
-- InterviewZen: https://interviewzen.com/interview/58drMKF

/* Although, using VS2019 editor there is an Npgsql NuGet package,
    but there is no postgresql SQL dialect, that I know of. */

/* Again, this is mostly an academic response to the interview question, with the
 caveat that for more production-ready code, we would want to consider database
 abstractions such as Person, Role (i.e. Student, Faculty, etc), and table structures,
 relationships, etc, along these lines. We would also most likely consider keys that
 are stronger than simple integers, i.e. UUID, along these lines. Furthermore, any more,
 we might consider alternatives to the classical relational database, such as JSON or BSON
 based document databases, which are more than adequate and up to the challenge these days,
 and are even quite good at capturing event driven systems. */

/*
    2. You are asked to design a relational database for a college's course registration
    system. The system has the following requirements:
      - A course can only be assigned one teacher
      - A teacher can teach many courses
      - A student can take many courses
      - A course can have many students

    (a) Based on the above requirements, show your database design. List all the tables
    in your design and include primary and foreign keys for each table.

    (b) Based on your design in part (a), write the SQL statement that will output all
    courses that have more than 100 students registered.
 */

-- using postgresql to exercise it, consult with vendor docs for whichever SQL dialect is involved.
create table course (
    id int not null generated always as identity
    , course_name varchar(128) not null
    , constraint pk_course primary key (id)
);

create table teacher (
    id int not null generated always as identity
    , teacher_name varchar(128) not null
    , constraint pk_teacher primary key (id)
);

create table student (
    id int not null generated always as identity
    , student_name varchar(128) not null
    , constraint pk_student primary key (id)
);

-- joins the course table with the teacher table
create table teacher_workload (
    course_id int not null
    , teacher_id int not null -- but do not restrict the teacher workload itself
    , constraint uq_course unique (course_id) -- each course instructed by only one teacher, meaning course may only appear once in the workload
    , constraint fk_course foreign key (course_id) references course (id)
        on update cascade on delete cascade
    , constraint fk_teacher foreign key (teacher_id) references teacher (id)
        on update cascade on delete cascade
);

/* do not restrict how many students a course may be registered with,
 nor how many courses a student may register with */
create table student_registration (
    course_id int not null
    , student_id int not null
    , constraint fk_student foreign key (student_id) references student (id)
        on update cascade on delete cascade
    , constraint fk_course foreign key (course_id) references course (id)
        on update cascade on delete cascade
);
