select exists(select id
              from employees
              where phone = @phone)