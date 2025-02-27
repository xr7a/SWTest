insert into employees (name, surname, phone, company_id, department_id)
values (@name, @surname, @phone, @companyId, @departmentId) returning id