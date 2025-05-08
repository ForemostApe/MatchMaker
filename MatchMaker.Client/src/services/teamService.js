import api from "./axiosConfig";

const getAllTeams = async () => {
    try {
        const response = await api.get("/Team/");
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to fetch teams');
        } else if (error.request) {
            console.error('No response: ', error.request);
            throw new Error('No response from server');
        } else {
            console.error('Request error: ', error.message);
            throw new Error('Failed to make request');
        }
    }
}

const getTeamById = async (teamId) => {
    try {
        const response = await api.get(`Team/id/${teamId}`)
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to fetch team');
        } else if (error.request) {
            console.error('No response: ', error.request);
            throw new Error('No response from server');
        } else {
            console.error('Request error: ', error.message);
            throw new Error('Failed to make request');
        }
    }
}

export default {
    getAllTeams,
    getTeamById
}