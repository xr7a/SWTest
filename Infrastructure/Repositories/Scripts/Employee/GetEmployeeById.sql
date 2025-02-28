select
    e.id,
    e.name,
    e.surname,
    e.phone,
    e.company_id as "CompanyId",
    e.department_id as "DepartmentId",
    p.type as "PassportType",
    p.number as "PassportNumber"
from employees e
left join passports p on e.department_id = p.id
where e.id = @id