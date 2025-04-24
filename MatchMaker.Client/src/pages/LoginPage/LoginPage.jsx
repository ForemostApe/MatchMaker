import { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import LoginForm from "../../components/LoginForm/LoginForm";
import { useAuth } from "../../context/AuthContext/AuthContext";

const LoginPage = () => {
    const navigate = useNavigate();
    const { user, login } = useAuth();
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (user) {
            navigate('/home');
        }
    }, [user, navigate]);

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

    if (user) {
        return null;
    }

    return (
        <div className="min-h-screen flex items-center justify-center">
            <LoginForm onSubmit={handleLogin} />
            <div className="text-sm text-gray-600">
                Don't have an account?{" "}
                <Link to="/register" className="text-blue-600 hover:underline">
                    Register here
                </Link>
            </div>
        </div>
    );
};

export default LoginPage;