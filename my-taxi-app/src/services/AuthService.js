const AuthService = {
    // User login
    login: async (username, password) => {
        try {
            const response = await fetch('http://localhost:5009/api/account/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, password }),
            });

            if (response.ok) {
                const data = await response.json();
                // Save JWT token to localStorage
                localStorage.setItem('jwtToken', data.token);
                return true;  // Login successful
            } else {
                const errorMessage = await response.text();
                console.error('Login failed:', errorMessage);
                return false;  // Login failed
            }
        } catch (error) {
            console.error('Login failed due to network error:', error);
            return false;
        }
    },

    // User registration
    register: async (formData) => {
        try {
            const response = await fetch('http://localhost:5009/api/account/register', {
                method: 'POST',
                // Don't set 'Content-Type' manually, the browser will set the correct boundary for FormData
                body: formData,  // Send the FormData object directly
            });

            if (response.ok) {
                return true;  // Registration successful
            } else {
                const errorMessage = await response.text();
                console.error('Registration failed:', errorMessage);
                return false;  // Registration failed
            }
        } catch (error) {
            console.error('Registration failed due to network error:', error);
            return false;
        }
    },

    // Logout (Remove JWT token from localStorage)
    logout: () => {
        localStorage.removeItem('jwtToken');  // Remove the JWT token from localStorage
    },

    // Get the current user's profile (requires authentication)
    getProfile: async () => {
        const token = localStorage.getItem('jwtToken');
        if (!token) {
            return null;  // No token, no logged-in user
        }

        try {
            const response = await fetch('http://localhost:5009/api/account/profile', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,  // Send the JWT token in the Authorization header
                },
            });

            if (response.ok) {
                return await response.json();  // Return the user's profile data
            } else {
                const errorMessage = await response.text();
                console.error('Failed to retrieve profile:', errorMessage);
                return null;  // Failed to retrieve profile
            }
        } catch (error) {
            console.error('Failed to get profile due to network error:', error);
            return null;
        }
    },

    // Update user profile (requires authentication)
    updateProfile: async (formData) => {
        const token = localStorage.getItem('jwtToken');
        if (!token) {
            return null;  // No token, no logged-in user
        }

        try {
            const response = await fetch('http://localhost:5009/api/account/profile', {
                method: 'PUT',
                headers: {
                    'Authorization': `Bearer ${token}`,  // Send the JWT token in the Authorization header
                },
                body: formData,  // Send the FormData object for updating profile (supports file uploads)
            });

            if (response.ok) {
                return true;  // Profile updated successfully
            } else {
                const errorMessage = await response.text();
                console.error('Profile update failed:', errorMessage);
                return false;  // Profile update failed
            }
        } catch (error) {
            console.error('Failed to update profile due to network error:', error);
            return false;
        }
    },

    // Check if the user is authenticated
    isAuthenticated: () => {
        const token = localStorage.getItem('jwtToken');
        return !!token;  // Check if token exists (returns true if token exists, false otherwise)
    },

    // Get the stored JWT token (if needed)
    getToken: () => {
        return localStorage.getItem('jwtToken');
    },
};

export default AuthService;
