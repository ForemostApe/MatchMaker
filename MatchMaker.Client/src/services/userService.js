import api from "./axiosConfig";

const registerUser = async (formData) => {
  const response = await api.post("/User/", {
    Password: formData.password,
    Email: formData.email,
    FirstName: formData.firstName,
    LastName: formData.lastName
  });
  return response.data;
}

const getAllUsers = async () => {
  try {
    const response = await api.get("User/");
    return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to fetch users');
        } else if (error.request) {
            console.error('No response: ', error.request);
            throw new Error('No response from server');
        } else {
            console.error('Request error: ', error.message);
            throw new Error('Failed to make request');
        }
    }
}

const getUserById = async (userId) => {
    try {
        const response = await api.get(`User/id/${userId}`);
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to fetch user');
        } else if (error.request) {
            console.error('No response: ', error.request);
            throw new Error('No response from server');
        } else {
            console.error('Request error: ', error.message);
            throw new Error('Failed to make request');
        }
    }
}

const getUsersByRole = async (userRole) => {
  try {
    const response = await api.get(`User/role/${userRole}`);
    return response.data;
  } catch (error) {
    if (error.response) {
        console.error('Error response: ', error.response.data);
        throw new Error(error.response.data.title || 'Failed to fetch users');
    } else if (error.request) {
        console.error('No response: ', error.request);
        throw new Error('No response from server');
    } else {
        console.error('Request error: ', error.message);
        throw new Error('Failed to make request');
    }
  }
}

const updateUser = (profileData) => {
  try {
    return api.patch("/User/", profileData);
    } catch (error) {
      if (error.response) {
          console.error('Error response: ', error.response.data);
          throw new Error(error.response.data.title || 'Failed to update user');
      } else if (error.request) {
          console.error('No response: ', error.request);
          throw new Error('No response from server');
      } else {
          console.error('Request error: ', error.message);
          throw new Error('Failed to make request');
      }
  }
};

export default {
  registerUser,
  getAllUsers,
  getUserById,
  getUsersByRole,
  updateUser
};