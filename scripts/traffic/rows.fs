\ rows.fs - the words the cars' routes are written in (scripts/events/car-routes.fs): kept apart so their names
\ (lane, roads, parks ...) don't hide the language's elsewhere.
IN: traffic.rows
USING: traffic ;

variable writing
: car-route ( n -- )   \ its row starts here
    here swap cells car-routes + !  here writing !  car-route-row allot ;
: lane ( node -- )  writing @ route-lane ! ;
: roads ( node.. count -- )   \ the nodes, in the order written
    {: n :}  n writing @ #route-nodes !  here {: at :}  n cells allot
    n 0 ?do  at n 1- i - cells + !  loop ;
: parks ( node -- )  ends-parked writing @ route-ends !  writing @ route-end-at ! ;
: loops-to ( i -- )  ends-going-round writing @ route-ends !  writing @ route-end-at ! ;
: end-car-route ( -- ) ;
