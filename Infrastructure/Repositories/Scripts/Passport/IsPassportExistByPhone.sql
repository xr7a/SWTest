select exists(select id
              from passports
              where number = @number)