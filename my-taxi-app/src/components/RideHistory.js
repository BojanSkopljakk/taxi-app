import React, { useState, useEffect } from 'react';
import RideService from '../services/RideService';

const RideHistory = () => {
    const [rideHistory, setRideHistory] = useState([]);

    useEffect(() => {
        const fetchHistory = async () => {
            const history = await RideService.getRideHistory();
            setRideHistory(history);
        };

        fetchHistory();
    }, []);

    return (
        <div>
            <h2>Ride History</h2>
            {rideHistory.length > 0 ? (
                <ul>
                    {rideHistory.map((ride) => (
                        <li key={ride.id}>
                            {ride.pickupLocation} to {ride.dropoffLocation} - {ride.status}
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No ride history available.</p>
            )}
        </div>
    );
};

export default RideHistory;
