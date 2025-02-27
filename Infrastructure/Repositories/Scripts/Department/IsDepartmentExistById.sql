select exists(select id 
              from departments
              where id = @id)