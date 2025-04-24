import RegistrationForm from "../../components/RegistrationForm/RegistrationForm";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import authService from "../../services/authService";

const RegistrationPage = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleRegister = async (formData) => {
    try {
      setLoading(true);
      await authService.register(formData);
      alert("Registration successful!");
      navigate("/");
    } catch (error) {
      console.error("Registration failed:", error);
      alert("Registration failed.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center">
      <RegistrationForm onSubmit={handleRegister} loading={loading} />
    </div>
  );
};

export default RegistrationPage;
