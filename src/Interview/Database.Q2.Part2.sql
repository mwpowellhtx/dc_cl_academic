
-- for this response , we also require a bit of test data in order to verify expected behavior

-- insert some faculty, students, courses...
insert into teacher (teacher_name) values
    ('sarah')
    , ('theiland')
    , ('ulma')
    , ('victor')

insert into student (student_name) values
    ('amy')
    , ('billy')
    , ('charles')
    , ('david')
    , ('emily')

insert into course (course_name) values
    ('101')
    , ('102')
    , ('103')
    , ('201')
    , ('202')
    , ('203')
    , ('301')
    , ('302')
    , ('303')
    , ('401')
    , ('402')
    , ('403')

-- assign course workload to faculty
insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'sarah' and c.course_name = '101';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'sarah' and c.course_name = '201';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'sarah' and c.course_name = '301';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'sarah' and c.course_name = '401';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'theiland' and c.course_name = '102';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'theiland' and c.course_name = '202';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'theiland' and c.course_name = '302';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'theiland' and c.course_name = '402';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'ulma' and c.course_name = '103';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'ulma' and c.course_name = '203';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'victor' and c.course_name = '303';

insert into teacher_workload (course_id, teacher_id)
    select c.id, t.id from teacher t, course c
    where t.teacher_name = 'victor' and c.course_name = '403';

-- register students for courses
insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'amy' and c.course_name = '101';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'amy' and c.course_name = '102';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'amy' and c.course_name = '103';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'billy' and c.course_name = '101';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'billy' and c.course_name = '102';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'charles' and c.course_name = '201';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'charles' and c.course_name = '202';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'david' and c.course_name = '203';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'david' and c.course_name = '301';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'david' and c.course_name = '302';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'emily' and c.course_name = '303';

insert into student_registration (course_id, student_id)
    select c.id, s.id from student s, course c
    where s.student_name = 'emily' and c.course_name = '401';

-- note, verified using smaller counts; should be able to rinse and repeat for other counts, i.e. 100

-- then identify the courses with sufficient enrollment
select c.course_name, count(c.id) as enrollment
    from course c join student_registration r on r.course_id = c.id
    group by c.id
    having count(c.id) >= 100;
