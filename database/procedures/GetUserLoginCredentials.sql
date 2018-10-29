CREATE TYPE tt."Credentials" AS ("Password" VARCHAR, "Salt" VARCHAR);

CREATE OR REPLACE FUNCTION tt."GetUserLoginCredentials"(emailAddress TEXT)
RETURNS tt."Credentials" AS $$

DECLARE
    result_record tt."Credentials";

BEGIN
    SELECT  "Password",
            "Salt"
    INTO    result_record."Password",
            result_record."Salt"
    FROM    tt."TrainerCredentials" C,
            tt."Trainer" T
    WHERE   T."TrainerId" = C."TrainerId"   AND
            T."EmailAddress" = emailAddress; 

    RETURN  result_record;
END;
$$  LANGUAGE plpgsql SECURITY DEFINER;