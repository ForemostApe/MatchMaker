import api from './axiosConfig';

const register = async (formData) => {
  const response = await api.post("/User/", {
    Password: formData.password,
    Email: formData.email,
    FirstName: formData.firstName,
    LastName: formData.lastName
  });
  return response.data;
}

const login = async (credentials) => {
  const response = await api.post("/Auth/login", {
    Email: credentials.email,
    Password: credentials.password
  });

  if (response.data?.data?.accessToken) {
    api.defaults.headers.common["Authorization"] = `Bearer ${response.data.data.accessToken}`;
  }
  return response.data;
};

const logout = async () => {
  await api.post("/Auth/logout");
  delete api.defaults.headers.common["Authorization"];
};

const refresh = async () => {
  const response = await api.post("/Auth/refresh");
  api.defaults.headers.common["Authorization"] = `Bearer ${response.data.accessToken}`;
  return response.data;
};

export default {
  register,
  login,
  logout,
  refresh,
};
