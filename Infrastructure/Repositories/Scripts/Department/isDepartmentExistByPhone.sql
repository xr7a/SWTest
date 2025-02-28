select exists(select id
              from departments
              where phone = @phone)