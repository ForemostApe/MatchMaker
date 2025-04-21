import LoginForm from "../../components/LoginForm/LoginForm";

const LoginPage = () => {
  const handleLogin = (credentials) => {
    console.log("Logging in with:", credentials);
  };

  return (
    <div className="min-h-screen flex items-center justify-center">
      <LoginForm onSubmit={handleLogin} />
    </div>
  );
};

export default LoginPage;