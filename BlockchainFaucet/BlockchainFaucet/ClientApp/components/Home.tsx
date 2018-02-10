import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Button, Col, FormGroup, ControlLabel, FormControl } from 'react-bootstrap'

export class Home extends React.Component<RouteComponentProps<{}>, {}> {
    onChangeHandler(event: Event)  {
        // let data = {};
        // data[event.target.name] = event.target.value
        // this.setState(data);
    }

    onSubmitHandler(event: Event) {
        event.preventDefault();
        console.log("submit")
        // this.behavior.login(this.state.username, this.state.password, function () {
        //     this.props.history.push('/');
        // }.bind(this));
    }

    public render() {
        return (
            <section className='row'>
                <Col className='col-sm-12'>
                    <h1 className='lead'>Please enter your address:</h1>
                    <form onSubmit={this.onSubmitHandler.bind(this)}>
                         <FormGroup controlId='address'>
                            <ControlLabel>Address</ControlLabel>
                            <FormControl required onChange={this.onChangeHandler.bind(this)} />
                        </FormGroup>
                        <Button type='submit'>Claim</Button>
                    </form>
                </Col>
            </section>
        )
    }
}
