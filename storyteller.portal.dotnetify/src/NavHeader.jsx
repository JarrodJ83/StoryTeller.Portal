import React, { Component } from 'react';
import dotnetify from 'dotnetify';
import { Button, Grid, Row, Col, Navbar, Nav, NavItem, NavDropdown, MenuItem, DropdownMenu } from 'react-bootstrap';
class NavHeader extends Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("NavHeader", this);
        this.state = { Apps: [] };
    }
    render() {
        return (
            <Navbar inverse collapseOnSelect>
                <Navbar.Header>
                    <Navbar.Brand>
                        <a href="#">StoryTeller Portal</a>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    <Nav>
                        <NavItem eventKey={1} href="#">Home</NavItem>
                        <NavDropdown eventKey={3} title="Apps" id="basic-nav-dropdown">
                            {this.state.Apps.map(app => <MenuItem eventKey={app.id}>{app.Name}</MenuItem>)}
                        </NavDropdown>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}

export default NavHeader;
