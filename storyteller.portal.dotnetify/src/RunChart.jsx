import React, { Component } from 'react';
import dotnetify from 'dotnetify';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, ResponsiveContainer, Tooltip, Legend } from 'recharts';

class RunChart extends Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("RunChart", this);
        this.state = { Stats: [] };
    }
    componentWillUnmount() {
        this.vm.$destroy();
    }
    render() {
        return (

            <ResponsiveContainer width="100%" height={200}>
            <LineChart data={this.state.Stats} margin={{ top: 5, right: 5, left: 5, bottom: 5 }} >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="Day" />
                <YAxis />
                <Line type="monotone" dataKey="Failed" stroke="#FF0000" label="Failed" />
                <Line type="monotone" dataKey="Successful" stroke="#00FF21" label="Passed" />
                
                <Tooltip />
                <Legend />
            </LineChart>
            </ResponsiveContainer>
        );
    }
}

export default RunChart;
