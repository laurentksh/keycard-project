import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link
} from "react-router-dom";
import HomeComponent from "./component/HomeComponent";
import LoginComponent from "./component/loginComponent";

export default function App() {
  const [loginMenu, isLoginMenu] = React.useState(false);
  const [homeMenu, isHomePage] = React.useState(false);

  function MenuLink(props: any) {
    const isHome = props.isHomePage;
    const isLogin = props.isLoginPage;

    if (loginMenu) 
    {
      isHomePage(false);
      return <LoginComponent/>
    } else if (homeMenu) {
      isLoginMenu(false);
      return <HomeComponent/>
    } else {
      return <HomeComponent/>
    }
  }

  return (
    <Router>
      <div>
        <nav>
          <ul>
            <li>
              <Link to="/">Home</Link>
            </li>
            <li>
              <Link to="/login">Login</Link>
            </li>
          </ul>
        </nav>

        {/* A <Switch> looks through its children <Route>s and
            renders the first one that matches the current URL. */}
        <Routes>
          <Route path="/" element={< HomeComponent />}></Route>
          <Route path="/login" element={< LoginComponent />} ></Route>
        </Routes>

      </div>
    </Router>
  );
}