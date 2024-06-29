import React from "react";
import "./style.css"
import SideBar from "../sidebar";
const Layout = ({ children }) => {
    return (
      <section className="fullBody">
        <article className="sidebarWrapper">

        <SideBar Selected={0}/>
        </article>
        <main className="mainClass">{children}</main>
      </section>
    );
  };


export default Layout;