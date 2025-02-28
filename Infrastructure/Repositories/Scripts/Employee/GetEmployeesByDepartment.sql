select
    e.id,
    e.name,
    e.surname,
    e.phone,
    e.company_id,
    p.type as "PassportType",
    p.number as "PassportNumber",
    d.name as "DepartmentName",
    d.phone as "DepartmentPhone"
from employees e
         left join passports p on e.id = p.employee_id
         left join departments d on e.department_id = d.id
where e.department_id = @departmentId;
