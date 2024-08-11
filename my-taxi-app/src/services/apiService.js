import axios from 'axios';

// Set the base URL for your API
const API_BASE_URL = 'https://your-backend-url/api';

export const registerUser = async (userData) => {
  return axios.post(`${API_BASE_URL}/users/register`, userData);
};

export const loginUser = async (credentials) => {
  return axios.post(`${API_BASE_URL}/users/login`, credentials);
};

export const getUserProfile = async (token) => {
  return axios.get(`${API_BASE_URL}/users/profile`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
};

// Add more API functions as needed
