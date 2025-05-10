import api from "./axiosConfig";

const updateUser = (profileData) => {
    return api.patch("/User/", profileData);
};

export default {
  updateUser,
};