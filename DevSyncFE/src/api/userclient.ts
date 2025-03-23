import { api } from "./api";

export interface LoginResponse {
    token: string;
    iss: string;
    expires: string;
}

/**
 *
 * @param email
 * @param password
 * @throws Error if login fails
 */
export const loginUser = async (email: string, password: string): Promise<LoginResponse> => {
  const response = await api.post("/User/login", {
    email,
    password,
  });

  if (response.status !== 200) {
    throw new Error("Invalid credentials");
  }

  return response.data;
};

export const getProfile = async () => {
  const { data } = await api.get("/User/me");
  return data;
};
