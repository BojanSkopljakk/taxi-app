const AdminService = {
    getAllRides: async () => {
        const response = await fetch('/api/admin/rides', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
            }
        });
        return response.json();
    },

    verifyDriver: async (driverId) => {
        const response = await fetch(`/api/admin/verify-driver/${driverId}`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
            }
        });
        return response.json();
    },

    blockDriver: async (driverId) => {
        const response = await fetch(`/api/admin/block-driver/${driverId}`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
            }
        });
        return response.json();
    },

    unblockDriver: async (driverId) => {
        const response = await fetch(`/api/admin/unblock-driver/${driverId}`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('jwtToken')}`
            }
        });
        return response.json();
    }
};

export default AdminService;
