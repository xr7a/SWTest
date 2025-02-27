select
    e.id, e.name, e.surname, e.phone, e.company_id,
    p.type as passport_type, p.number as passport_number,
    d.name as department_name, d.phone as department_phone
from employees e
         left join passports p on e.id = p.employee_id
         left join departments d on e.departmentId = d.id
where e.id = @departmentId;