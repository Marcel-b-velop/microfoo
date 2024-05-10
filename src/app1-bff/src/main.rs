use hyper::{Body, Request, Response, Server};
use hyper::service::{make_service_fn, service_fn};
use hyper_staticfile::Static;
use std::convert::Infallible;
use std::net::SocketAddr;

async fn serve(req: Request<Body>, static_: Static) -> Result<Response<Body>, Infallible> {
    hyper_staticfile::resolve(&static_, &req).await.unwrap()
}

async fn run(addr: SocketAddr) {
    let static_ = Static::new("path/to/your/static/files");

    let make_service = make_service_fn(move |_| {
        let static_ = static_.clone();
        async move {
            Ok::<_, Infallible>(service_fn(move |req| serve(req, static_.clone())))
        }
    });

    let server = Server::bind(&addr).serve(make_service);

    if let Err(e) = server.await {
        eprintln!("server error: {}", e);
    }
}

fn main() {
    let addr = SocketAddr::from(([127, 0, 0, 1], 3000));

    let rt = tokio::runtime::Runtime::new().unwrap();
    rt.block_on(run(addr));
}
