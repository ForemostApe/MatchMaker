import api from "./axiosConfig";


const createGame = async(gameData) => {
    try {
        const response = await api.post("/Game/", gameData);
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to create game');
        } else if (error.request) {
            console.error('No response: ', error.request);
            throw new Error('No response from server');
        } else {
            console.error('Request error: ', error.message);
            throw new Error('Failed to make request');
        }
    }
}

const getAllGames = async () => {
    try {
        const response = await api.get("/Game/");
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to fetch games');
        } else if (error.request) {
            console.error('No response: ', error.request);
            throw new Error('No response from server');
        } else {
            console.error('Request error: ', error.message);
            throw new Error('Failed to make request');
        }
    }
}

const getGameById = async (gameId) => {
    try {
        const response = await api.get(`Game/id/${gameId}`)
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to fetch game');
        } else if (error.request) {
            console.error('No response: ', error.request);
            throw new Error('No response from server');
        } else {
            console.error('Request error: ', error.message);
            throw new Error('Failed to make request');
        }
    }
}

const updateGame = async (gameData) => {
    try {
        const response = await api.patch("/Game", gameData);
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to update game');
        } else if (error.request) {
            console.error('No response: ', error.request);
            throw new Error('No response from server');
        } else {
            console.error('Request error: ', error.message);
            throw new Error('Failed to make request');
        }
    }
};

const submitUserResponse = async (gameId, accepted) => {
        try {
        const response = await api.post("Game/userResponse", { gameId, accepted });
        return response.data;
    } catch (error) {
        if (error.response) {
            console.error('Error response: ', error.response.data);
            throw new Error(error.response.data.title || 'Failed to update game');
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
    createGame,
    getAllGames,
    getGameById,
    updateGame,
    submitUserResponse
}