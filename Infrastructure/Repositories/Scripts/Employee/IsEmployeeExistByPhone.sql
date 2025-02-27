select exists(select id
              from employees
              where id = @id)