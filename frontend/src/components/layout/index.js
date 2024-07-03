import React, { useEffect, useState } from "react";
import "./style.css";
import SideBar from "../sidebar";
import { useLocation } from "react-router-dom";

const Layout = ({ children }) => {
  const [selected, setSelectedButton] = useState("/");
  const location = useLocation();
  useEffect(() => {
    setSelectedButton(location.pathname);
  }, [location.pathname]);
  return (
    <section className="fullBody">
      <main className="mainClass">
      <article className="sidebarWrapper">
        <SideBar Selected={selected} />
      </article>
        
        <section className="children">
          
        {children}
        </section>
        </main>
    </section>
  );
};

export default Layout;
