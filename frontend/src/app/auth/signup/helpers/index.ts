import * as Yup from "yup";

export const formDefaultValues = {
  firstName: "",
  lastName: "",
  email: "",
  gender: "",
  password: "",
  passwordConfirmation: "",
};

export const validators = {
  firstName: Yup.string().required("First name is required"),
  lastName: Yup.string().required("Last name is required"),
  email: Yup.string().email("Email is invalid").required("Email is required"),
  gender: Yup.string().oneOf(["male", "female", "other"], "Select a gender"),
  password: Yup.string()
    .required("Required")
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[a-zA-Z\d@$!%*?&]{8,}$/,
      "Password must be at least 8 characters long and contain at least one number, one uppercase letter, one lowercase letter and one special character from @$!%*?&"
    ),
  passwordConfirmation: Yup.string()
    .required("Required")
    .oneOf([Yup.ref("password")], "Passwords must match"),
};