import { api } from './axiosConfig';

export const AuthService = {
  login: async (credentials) => {
    try {
      const response = await api.post('/Auth/login', credentials, {
        withCredentials: true

      });
      return response.data;
    } catch (error) {
      throw new Error(
        error.response?.data?.message || 
        'Login failed. Please check your credentials and try again.'
      );
    }
  },

  logout: async () => {
    try {
      await api.post('/Auth/logout', {}, {
        withCredentials: true
      });
    } catch (error) {
      console.error('Logout failed:', error);
      throw new Error('Failed to logout. Please try again.');
    }
  }
};