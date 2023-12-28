"use client";
import { FormsFieldsContainer } from "@/components/AuthContainer/styled-components";
import MuiLocalizationProvider from "@/components/MuiLocalizationProvider";
import { Box, Button, Stack, TextField } from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers";
import useSignUpUtils from "../hooks";
import MuiTanTextField from "@/components/MuiTanTextField";

const Form = () => {
  const { form } = useSignUpUtils();

  return (
    <form.Provider>
      <Box
        component="form"
        onSubmit={(e) => {
          e.preventDefault();
          e.stopPropagation();
          form.handleSubmit();
        }}
      >
        <FormsFieldsContainer>
          <form.Field name="firstName">
            {(field) => (
              <MuiTanTextField field={field} label="First name" size="small" />
            )}
          </form.Field>
          <form.Field name="lastName">
            {(field) => (
              <MuiTanTextField field={field} label="Last name" size="small" />
            )}
          </form.Field>
          <form.Field name="email">
            {(field) => (
              <MuiTanTextField
                field={field}
                label="Email"
                type="email"
                size="small"
              />
            )}
          </form.Field>
          <MuiLocalizationProvider>
            <DatePicker
              label="Date of birth"
              // TODO: make the datepicker small like the other fields
              // sx={{
              //   "& .MuiOutlinedInput-input": {
              //     py: 1.0625,
              //   },
              //   "& .MuiFormLabel-root": {
              //     transform: "translate(14px, 8.5px) scale(1)",
              //   },
              // }}
            />
          </MuiLocalizationProvider>
          <TextField label="Gender" size="small" />
          <form.Field name="password">
            {(field) => (
              <MuiTanTextField
                field={field}
                label="Password"
                type="password"
                size="small"
              />
            )}
          </form.Field>
          <form.Field name="passwordConfirmation">
            {(field) => (
              <MuiTanTextField
                field={field}
                label="Confirm password"
                type="password"
                size="small"
              />
            )}
          </form.Field>
        </FormsFieldsContainer>

        <Stack alignItems="center" mt={5}>
          <form.Subscribe selector={(s) => s.isSubmitting}>
            {(isSubmitting) => (
              <Button variant="contained" type="submit" disabled={isSubmitting}>
                Sign Up
              </Button>
            )}
          </form.Subscribe>
        </Stack>
      </Box>
    </form.Provider>
  );
};

export default Form;
