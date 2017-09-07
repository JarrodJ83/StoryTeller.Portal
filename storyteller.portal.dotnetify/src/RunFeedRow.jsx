import React from 'react';
import dotnetify from 'dotnetify';
import { Table, Glyphicon } from 'react-bootstrap';

class RunFeedRow extends React.PureComponent  {
    constructor(props) {
        super(props);
        this.state = { Run: this.props.Run };
    }
    componentWillUnmount() {
        this.vm.$destroy();
    }
    shouldComponentUpdate(nextProps) {
        console.log("RunFeedRow shouldComponentUpdate");
        return (nextProps.Run.FailureCount !== this.props.Run.FailureCount
            || nextProps.Run.SuccessCount !== this.props.Run.SuccessCount
            || nextProps.Run.Finished !== this.props.Run.Finished);
    }
    render() {
        return (<tr key={this.state.Run.Id}>
            <td>{this.state.Run.Name}</td>
                    <td sytle="color: green;">{this.state.Run.AppName}</td>
                    <td><span>(Pass: {this.state.Run.SuccessfulCount} </span><span sytle="color: green;">Fail: {this.state.Run.FailureCount} Total: {this.state.Run.TotalCount})</span></td>

                    <td>
                    ...
                    </td>
                    <td>..</td>
                </tr >);
    }
}
export default RunFeedRow;