import api from "./axiosConfig";

const getAllTeams = async () => {
    const response = await api.get("/Team/");

    if (response.data?.data?.accessToken) {
        api.defaults.headers.common["Authorization"] = `Bearer ${response.data.data.accessToken}`;
      }
      return response.data;
}