import { useState } from "react";
import { useNavigate } from "react-router-dom";
import LoginForm from "../../components/LoginForm/LoginForm";
import { useAuth } from "../../context/AuthContext/AuthContext";

const LoginPage = () => {
    const navigate = useNavigate();
    const { login } = useAuth();
    const [loading, setLoading] = useState(false);

    const handleLogin = async (credentials) => {
        try {
            setLoading(true);
            await login(credentials);
            navigate('/home');
        } catch (error) {
            alert('Login failed.') || 'Login failed.'
        }
        finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center">
            <LoginForm onSubmit={handleLogin} />
        </div>
    );
};

export default LoginPage;