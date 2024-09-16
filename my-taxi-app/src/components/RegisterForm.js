import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthService from '../services/AuthService';

const RegisterForm = () => {
    const [formData, setFormData] = useState({
        username: '',
        email: '',
        password: '',
        confirmPassword: '',
        fullName: '',
        dateOfBirth: '',
        address: '',
        profilePicture: null, // Add this if you are handling profile pictures
    });

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value, files } = e.target;
        if (name === "profilePicture") {
            setFormData({ ...formData, profilePicture: files[0] }); // Handle file input
        } else {
            setFormData({ ...formData, [name]: value });
        }
    };

    const handleRegister = async (e) => {
        e.preventDefault();

        // Convert the form data to FormData object
        const formDataToSend = new FormData();
        formDataToSend.append('username', formData.username);
        formDataToSend.append('email', formData.email);
        formDataToSend.append('password', formData.password);
        formDataToSend.append('confirmPassword', formData.confirmPassword);
        formDataToSend.append('fullName', formData.fullName);
        formDataToSend.append('dateOfBirth', formData.dateOfBirth);
        formDataToSend.append('address', formData.address);
        if (formData.profilePicture) {
            formDataToSend.append('profilePicture', formData.profilePicture); // Append file if it exists
        }

        const success = await AuthService.register(formDataToSend);
        if (success) {
            navigate('/login');  // Redirect to login after successful registration
        } else {
            alert('Registration failed');
        }
    };

    return (
        <div>
            <h1>Register Form</h1>
            <form onSubmit={handleRegister}>
                <div>
                    <label>Username</label>
                    <input
                        type="text"
                        name="username"
                        value={formData.username}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Email</label>
                    <input
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Password</label>
                    <input
                        type="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Confirm Password</label>
                    <input
                        type="password"
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Full Name</label>
                    <input
                        type="text"
                        name="fullName"
                        value={formData.fullName}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Date of Birth</label>
                    <input
                        type="date"
                        name="dateOfBirth"
                        value={formData.dateOfBirth}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Address</label>
                    <input
                        type="text"
                        name="address"
                        value={formData.address}
                        onChange={handleChange}
                        required
                    />
                </div>

                {/* Add this if you need profile picture upload */}
                <div>
                    <label>Profile Picture</label>
                    <input
                        type="file"
                        name="profilePicture"
                        onChange={handleChange}
                    />
                </div>

                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default RegisterForm;
