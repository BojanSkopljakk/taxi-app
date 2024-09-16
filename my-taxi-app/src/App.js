import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';

// Components
import LoginForm from './components/LoginForm';
import RegisterForm from './components/RegisterForm';
import UserProfile from './components/UserProfile';
import UserDashboard from './components/UserDashboard';
import DriverDashboard from './components/DriverDashboard';
import AdminDashboard from './components/AdminDashboard';

// AuthService to check if the user is authenticated
import AuthService from './services/AuthService';

// Private Route wrapper to protect routes
const PrivateRoute = ({ children }) => {
    return AuthService.isAuthenticated() ? children : <Navigate to="/login" />;
};

// App Component
function App() {
    return (
        <Router>
            <div>
                {/* Define routes */}
                <Routes>
                    {/* Authentication */}
                    <Route path="/login" element={<LoginForm />} />
                    <Route path="/register" element={<RegisterForm />} />

                    {/* User Dashboard */}
                    <Route
                        path="/user/dashboard"
                        element={
                            <PrivateRoute>
                                <UserDashboard />
                            </PrivateRoute>
                        }
                    />

                    {/* Driver Dashboard */}
                    <Route
                        path="/driver/dashboard"
                        element={
                            <PrivateRoute>
                                <DriverDashboard />
                            </PrivateRoute>
                        }
                    />

                    {/* Admin Dashboard */}
                    <Route
                        path="/admin/dashboard"
                        element={
                            <PrivateRoute>
                                <AdminDashboard />
                            </PrivateRoute>
                        }
                    />

                    {/* Profile */}
                    <Route
                        path="/profile"
                        element={
                            <PrivateRoute>
                                <UserProfile />
                            </PrivateRoute>
                        }
                    />

                    {/* Default Route: Redirect to login */}
                    <Route path="*" element={<Navigate to="/login" />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
