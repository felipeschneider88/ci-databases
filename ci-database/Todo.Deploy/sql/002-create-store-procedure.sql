create or replace function web.get_todos(idInput INTEGER default 0 )
returns table(id int,
                title varchar(100),
	            completed boolean)
as $$
BEGIN
  IF idInput IS NULL OR idInput <= 0 THEN
    return query
    select t.id, t.title, t.completed from web.todos as t;
  ELSE
    return query    
    select t.id, t.title, t.completed from web.todos as t where t.id = idInput;
  END IF;
END;
$$ LANGUAGE 'plpgsql';