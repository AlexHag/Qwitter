import Home from "./Home";
import BankHome from "./BankHome";
import { useAuth } from "../Auth/AuthProvider";

function HomeRoute() {
  const auth = useAuth();

  if (auth.token) {
    return <BankHome />;
  }
  return <Home />;
}

export default HomeRoute;