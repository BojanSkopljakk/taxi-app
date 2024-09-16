import React, { useState, useEffect } from 'react';
import AdminService from '../services/AdminService';

const AdminDashboard = () => {
    const [allRides, setAllRides] = useState([]);
    const [pendingDrivers, setPendingDrivers] = useState([]);

    useEffect(() => {
        // Fetch all rides and pending driver verifications
        const fetchData = async () => {
            const rides = await AdminService.getAllRides();
            const drivers = await AdminService.getPendingVerifications();
            setAllRides(rides);
            setPendingDrivers(drivers);
        };

        fetchData();
    }, []);

    const handleVerifyDriver = async (driverId) => {
        const result = await AdminService.verifyDriver(driverId);
        if (result) {
            alert('Driver verified successfully');
            setPendingDrivers(pendingDrivers.filter((driver) => driver.id !== driverId));
        }
    };

    return (
        <div>
            <h2>All Rides</h2>
            {allRides.length > 0 ? (
                <ul>
                    {allRides.map((ride) => (
                        <li key={ride.id}>
                            {ride.pickupLocation} to {ride.dropoffLocation} - {ride.status}
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No rides available.</p>
            )}

            <h2>Pending Driver Verifications</h2>
            {pendingDrivers.length > 0 ? (
                <ul>
                    {pendingDrivers.map((driver) => (
                        <li key={driver.id}>
                            {driver.fullName} - {driver.licenseNumber}
                            <button onClick={() => handleVerifyDriver(driver.id)}>Verify</button>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No pending verifications.</p>
            )}
        </div>
    );
};

export default AdminDashboard;
